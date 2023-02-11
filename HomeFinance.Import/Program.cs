using HomeFinance.DataAccess;
//using HomeFinance.DataAccess.Sqlite;
using HomeFinance.DataAccess.MsSql;
using HomeFinance.Domain.DomainModels;
using HomeFinance.Domain.Enums;
using Microsoft.EntityFrameworkCore;

//var context = new HomeFinanceContext(new DbContextOptionsBuilder().UseSqlite($@"Data Source = D:\srcPrivate\HomeFinance\dbtest.db").UseLazyLoadingProxies().Options);
var context = new HomeFinanceContext(new DbContextOptionsBuilder().UseSqlServer("Server=.;Database=HomeFinance;Trusted_Connection=True;MultipleActiveResultSets=true").UseLazyLoadingProxies().Options);
var inputFile = "C:\\Users\\KirillKozlov\\Downloads\\2023-01-13.csv";
var userId= "acf45568-a967-4b9a-98a5-3186d5917693";
var gateway = new Gateway(context, new UserService(userId));

var models=File.ReadAllLines(inputFile).Skip(1).Select(i=>new Model(i)).Where(i=>i.Valid).ToList();

var wallets=models
    .Select(i => i.Wallet)
    .Concat(models.Select(i => i.WalletTo))
    .Where(i => !string.IsNullOrWhiteSpace(i))
    .Distinct()
    .Select(w=> new Wallet(Guid.NewGuid(), w!, "", ""))
    .ToList();


var tagsExpense = models
    .Where(t=>t.OperationType==OperationType.Expense)
    .SelectMany(i => i.Tags)
    .Where(i => !string.IsNullOrWhiteSpace(i))
    .Distinct()
    .Select(t=> new Tag(t, OperationType.Expense, 0))
    .ToList();
var tagsIncome = models
    .Where(t => t.OperationType == OperationType.Income)
    .SelectMany(i => i.Tags)
    .Where(i => !string.IsNullOrWhiteSpace(i))
    .Distinct()
    .Select(t => new Tag(t, OperationType.Income, 0))
    .ToList();

var tags=tagsExpense.Concat(tagsIncome).ToList();


await gateway.WalletRepository.AddRange(wallets);
await gateway.TagRepository.AddRange(tags);

var operations=models.Select(m => new Operation(
    Guid.NewGuid(),
    wallets.Single(i => i.Name == m.Wallet).Id! ?? throw new Exception(),
    m.OperationType,
    m.Tags,
    m.Amount,
    "",
    m.WalletTo != null ? wallets.Single(i => i.Name == m.WalletTo).Id : null,
    m.Date));

await gateway.OperationRepository.AddRange(operations);


class Model
{

    public bool Valid { get; }

    public DateTime Date { get;  }
    public string Wallet { get;  }
    public IEnumerable<string> Tags { get; }
    public double Amount { get;  }
    public OperationType OperationType { get; }
    public string? WalletTo { get;  }


    OperationType ParseOperationType(string text)
    {
        switch (text)
        {
            case "Расход":
                return OperationType.Expense;
            case "Доход":
                return OperationType.Income;
            case "Снятие перечисления":
                return OperationType.Transfer;
            case "Вклад перечисления":
                throw new NotSupportedException();
            default:
                throw new NotSupportedException();
        }
    }

    public Model(string csvLine)
    {
        var lineSplit=csvLine.Split(';');

        try
        {
            OperationType = ParseOperationType(lineSplit[6]);
            Valid= true;
        }
        catch
        {
            Valid = false;
        }
   

        Date = DateTime.Parse(lineSplit[0]);
        Wallet = lineSplit[1];

        Amount = double.Parse(lineSplit[5]);

        if (OperationType == OperationType.Transfer)
        {
            WalletTo= lineSplit[2];
            Tags =new List<string>();
        }
        else
        {
            var tags = new List<string> { lineSplit[2], lineSplit[3] };//, lineSplit[4] };
            Tags = tags.Select(i=>i.ToLower().Trim()).Distinct().Where(i => !string.IsNullOrWhiteSpace(i));
        }
        
    }
}
