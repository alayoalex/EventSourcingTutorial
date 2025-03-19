using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using EventSourcingTutorial.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventSourcingTutorial
{
    public class StudentDatabase
    {
        private readonly IAmazonDynamoDB _amazonDynamoDB = new AmazonDynamoDBClient("zzzzzzzzzzzzzzzzzzzzzzzzz", "xxxxxxxxxxxxxxxxxxxx", RegionEndpoint.USEast1);
        private const string TableName = "students";

        private static readonly JsonSerializerOptions SerializerSettings = new JsonSerializerOptions
        {
            AllowOutOfOrderMetadataProperties = true,
        };

        public async Task AppendAsync<T>(T @event) where T : Event
        {
            @event.CreatedAtUtc = DateTime.UtcNow;
            var eventAsJson = JsonSerializer.Serialize<Event>(@event);
            var itemAsDocument = Document.FromJson(eventAsJson);
            var itemAsAttributes = itemAsDocument.ToAttributeMap();

            var studentView = await GetStudentAsync(@event.StreamId) ?? new Student();
            studentView.Apply(@event);
            var studentAsJson = JsonSerializer.Serialize(studentView);
            var studentAsDocument = Document.FromJson(studentAsJson);
            var studentAsAttributes = studentAsDocument.ToAttributeMap();

            var transactionRequest = new TransactWriteItemsRequest
            {
                TransactItems = new List<TransactWriteItem>
                {
                    new TransactWriteItem
                    {
                        Put = new Put
                        {
                            TableName = TableName,
                            Item = itemAsAttributes
                        }
                    },
                    new TransactWriteItem
                    {
                        Put = new Put
                        {
                            TableName = TableName,
                            Item = studentAsAttributes
                        }
                    }
                }
            };

            await _amazonDynamoDB.TransactWriteItemsAsync(transactionRequest);
        }

        public async Task<Student?> GetStudentAsync(Guid studentId)
        {
            var request = new GetItemRequest
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"pk", new AttributeValue { S = $"{studentId.ToString()}_view" } },
                    {"sk", new AttributeValue { S = $"{studentId.ToString()}_view" } }
                }
            };

            var response = await  _amazonDynamoDB.GetItemAsync(request);
            if (response.Item == null)
                return null;

            var itemAsDocument = Document.FromAttributeMap(response.Item);
            var studentAsJson = itemAsDocument.ToJson();
            var student = JsonSerializer.Deserialize<Student>(studentAsJson);
            return student;
        }

        /*public async Task<Student?> GetStudentAsync(Guid studentId)
        {
            var queryRequest = new QueryRequest
            {
                TableName = TableName,
                KeyConditionExpression = "pk = :v_pk",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":v_pk", new AttributeValue { S = studentId.ToString() } }
                }
            };

            var queryResponse = await _amazonDynamoDB.QueryAsync(queryRequest);

            if (queryResponse.Items.Count == 0)
                return null;

            var studentEvents = queryResponse.Items.Select(item => Document.FromAttributeMap(item))
                .Select(document => JsonSerializer.Deserialize<Event>(document.ToJson(), SerializerSettings))
                .OrderBy(@event => @event!.CreatedAtUtc);

            var student = new Student();
            foreach (var @event in studentEvents)
            {
                student.Apply(@event!);
            }
            return student;
        }*/
    }
}
