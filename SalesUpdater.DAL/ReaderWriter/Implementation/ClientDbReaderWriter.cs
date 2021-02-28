using SalesUpdater.DAL.Repositories;
using SalesUpdater.Entity;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Interfaces.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using X.PagedList;

namespace SalesUpdater.DAL.ReaderWriter.Implementation
{
    public class ClientDbReaderWriter : IClientDbReaderWriter
    {
        private SalesContext Context { get; }

        private IClientRepository Clients { get; }

        public ClientDbReaderWriter(SalesContext context)
        {
            Context = context;

            var mapper = AutoMapper.CreateConfiguration().CreateMapper();

            Clients = new ClientRepository(Context, mapper);
        }

        public async Task<IPagedList<ClientDTO>> GetPagedListAsync(int number, int size,
            Expression<Func<ClientDTO, bool>> predicate = null,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return await Clients.GetPagedListAsync(number, size, predicate).ConfigureAwait(false);
        }

        public async Task<ClientDTO> GetAsync(int id)
        {
            return await Clients.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<ClientDTO> AddAsync(ClientDTO client)
        {
                if (await Clients.TryAddClientAsync(client).ConfigureAwait(false))
                {
                    await Clients.SaveAsync().ConfigureAwait(false);
                    return await GetAsync(await Clients.GetIdAsync(client.Name, client.Surname)
                        .ConfigureAwait(false)).ConfigureAwait(false);
                }
                else
                {
                    throw new ArgumentException("Client already exists!");
                }
        }

        public async Task<ClientDTO> UpdateAsync(ClientDTO client)
        {
                if (await Clients.DoesClientExistAsync(client).ConfigureAwait(false))
                    throw new ArgumentException("Client already exists!");

                var result = Clients.Update(client);
                await Clients.SaveAsync().ConfigureAwait(false);

                return result;
        }

        public async Task DeleteAsync(int id)
        {
                await Clients.DeleteAsync(id).ConfigureAwait(false);
                await Clients.SaveAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<ClientDTO>> FindAsync(Expression<Func<ClientDTO, bool>> predicate)
        {
            return await Clients.FindAsync(predicate).ConfigureAwait(false);
        }
    }
}