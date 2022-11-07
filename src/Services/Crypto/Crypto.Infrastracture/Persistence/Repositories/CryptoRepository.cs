using Crypto.Core.Interfaces;
using Crypto.Core.Queries.Response;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Crypto.Infrastracture.Persistence.Repositories
{
    public class CryptoRepository : BaseRepository<Core.Entities.Crypto>, ICryptoRepository
    {
        public CryptoRepository(CryptoDbContext context) : base(context)
        {

        }

        public async Task<CryptoResponseQuery> GetWithPrice(string symbol)
        {
            return await _context.Prices.Include(i => i.Crypto)
                                    .Where(i => i.Crypto.Symbol.ToLower() == symbol.ToLower())
                                    .OrderByDescending(i => i.CreatedOn)
                                    .Select(i => new CryptoResponseQuery
                                    {
                                        Id = i.CryptoId,
                                        Created = i.Crypto.CreatedOn,
                                        Name = i.Crypto.Name,
                                        Symbol = i.Crypto.Symbol,
                                        Description = i.Crypto.Description,
                                        Logo = i.Crypto.Logo,
                                        SourceCode = i.Crypto!.SourceCode,
                                        Website = i.Crypto!.WebSite,
                                        Price = i.Price
                                    }).FirstOrDefaultAsync();                               
        }

        public async Task<List<CryptoResponseQuery>> GetAllWithPrice()
        {
            var command = new CommandDefinition(
                @"SELECT C.Symbol, C.Name, C.Description, C.WebSite, C.SourceCode, 
	                    C.CreatedOn AS Created, C.Logo, IT.Price 
                  FROM [dbo].[Cryptos] AS C
                  JOIN (SELECT CryptoId, Price, 
                            ROW_NUMBER() OVER (PARTITION BY CryptoId ORDER BY CreatedOn DESC) AS Num 
		                FROM [dbo].[Prices]) IT
                  ON IT.CryptoId = C.Id
                  WHERE IT.Num = 1
                  ORDER BY C.Symbol ASC",
                parameters: null,
                transaction: _context.Database.CurrentTransaction?.GetDbTransaction(),
                commandTimeout: _context.Database.GetCommandTimeout() ?? 30
            );

            var connection = _context.Database.GetDbConnection();

            var result = await connection.QueryAsync<CryptoResponseQuery>(command);

            return result.ToList();
        }

        public async Task<List<string>> GetAllSymbols()
        {
            return await _context.Cryptos.Select(i => i.Symbol).ToListAsync();
        }

        public async Task<List<CryptoResponseQuery>> GetGroupWithPrices(List<string> symbols, int page, int take)
        {
            var parameters = new DynamicParameters();

            parameters.Add("Symbols", symbols);
            parameters.Add("Skip", (page - 1) * take);
            parameters.Add("Take", take);

            var command = new CommandDefinition(
                @"SELECT C.Symbol, C.Name, C.Description, C.WebSite, C.SourceCode, 
	                    C.CreatedOn AS Created, C.Logo, IT.Price 
                  FROM [dbo].[Cryptos] AS C
                  JOIN (SELECT CryptoId, Price, 
                            ROW_NUMBER() OVER (PARTITION BY CryptoId ORDER BY CreatedOn DESC) AS Num 
		                FROM [dbo].[Prices]) IT
                  ON IT.CryptoId = C.Id
                  WHERE IT.Num = 1 AND C.Symbol IN @Symbols
                  ORDER BY C.Symbol ASC
                  OFFSET @Skip ROWS
				  FETCH NEXT @Take ROWS ONLY",
                parameters: parameters,
                transaction: _context.Database.CurrentTransaction?.GetDbTransaction(),
                commandTimeout: _context.Database.GetCommandTimeout() ?? 30
            );

            var connection = _context.Database.GetDbConnection();

            var result = await connection.QueryAsync<CryptoResponseQuery>(command);

            return result.ToList();
        }

        public async Task<List<CryptoResponseQuery>> GetPageWithPrices(int page, int take)
        {
            var parameters = new DynamicParameters();

            parameters.Add("Skip", (page - 1) * take);
            parameters.Add("Take", take);

            var command = new CommandDefinition(
                @"SELECT C.Symbol, C.Name, C.Description, C.WebSite, C.SourceCode, 
	                    C.CreatedOn AS Created, C.Logo, IT.Price 
                  FROM [dbo].[Cryptos] AS C
                  JOIN (SELECT CryptoId, Price, 
                            ROW_NUMBER() OVER (PARTITION BY CryptoId ORDER BY CreatedOn DESC) AS Num 
		                FROM [dbo].[Prices]) IT
                  ON IT.CryptoId = C.Id
                  WHERE IT.Num = 1
                  ORDER BY C.Symbol ASC
                  OFFSET @Skip ROWS
				  FETCH NEXT @Take ROWS ONLY",
                parameters: parameters,
                transaction: _context.Database.CurrentTransaction?.GetDbTransaction(),
                commandTimeout: _context.Database.GetCommandTimeout() ?? 30
            );

            var connection = _context.Database.GetDbConnection();

            var result = await connection.QueryAsync<CryptoResponseQuery>(command);

            return result.ToList();
        }
    }
}
