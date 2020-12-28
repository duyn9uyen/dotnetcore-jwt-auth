using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetCustomer
{
    //IRequest of IEnumerable<string> says this query will return a list of strings.
    public class Query : IRequest<IEnumerable<string>> { }
    // public class Handler : RequestHandler<Query, IEnumerable<string>>
    // {
    //     private readonly FakeDataStore _db; //This would typically be the EF COR's dbContext class.
    //     public Handler(FakeDataStore db)
    //     {
    //         //DI what this handler may need. Here we need to have access to the fakeDataStore or dbcontext.
    //         _db = db;
    //     }
    //     protected override IEnumerable<string> Handle(Query request)
    //     {
    //         return _db.GetAllValues();
    //     }
    // }

    public class Handler : IRequestHandler<Query, IEnumerable<string>>
    {
        private readonly FakeDataStore _db; //This would typically be the EF COR's dbContext class.
        public Handler(FakeDataStore db)
        {
            //DI what this handler may need. Here we need to have access to the fakeDataStore or dbcontext.
            _db = db;
        }

        //Async version here
        public async Task<IEnumerable<string>> Handle(Query request, CancellationToken token)
        {
            return await _db.GetAllValuesAsync();
        }
    }
}