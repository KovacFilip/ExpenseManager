namespace API
{
    using System.Net;
    using System.Text.Json;
    using DB.models;
    using DB.UnitOfWork;
    using Helper.ObjectsToApi;

    public static class Functions
    {
        public static async Task Login(HttpContext context)
        {
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

            var requestBodyJson = JsonSerializer.Deserialize<LoginObject>(requestBody);

            if (
                requestBody == null
                || requestBodyJson!.Username == null
                || requestBodyJson.PasswordHash == null
            )
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Empty body");
            }
            else
            {
                Person? person;

                using (var uow = new UnitOfWork())
                {
                    person = await uow.Login(
                        requestBodyJson.Username,
                        requestBodyJson.PasswordHash
                    );
                }

                if (person != null)
                {
                    Console.WriteLine($"{person.Username}, {person.Id}, {person.Role}");

                    string resToFe = JsonSerializer.Serialize(person);
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    await context.Response.WriteAsync(resToFe);
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsync("Incorrect login credentials!");
                }
            }
        }

        public static async Task GetExpenses(HttpContext context)
        {
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

            var requestBodyJson = JsonSerializer.Deserialize<Person>(requestBody);

            if (requestBodyJson == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Invalid person");
                return;
            }

            List<Expense> expenses;
            using (var uow = new UnitOfWork())
            {
                expenses = await uow.GetExpenses(requestBodyJson.Id);
            }

            string resToFe = JsonSerializer.Serialize(expenses);
            Console.WriteLine($"{resToFe}");

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsync(resToFe);
        }

        public static async Task CreateExpense(HttpContext context)
        {
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var requestBodyJson = JsonSerializer.Deserialize<NewExpense>(requestBody);

            if (requestBodyJson == null || requestBodyJson.Category == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Invalid Expense");
                return;
            }

            using (var uow = new UnitOfWork())
            {
                await uow.AddExpense(
                    requestBodyJson.Category,
                    requestBodyJson.Price,
                    requestBodyJson.PersonId
                );
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            await context.Response.WriteAsync("Created new expense");
        }
    }
}
