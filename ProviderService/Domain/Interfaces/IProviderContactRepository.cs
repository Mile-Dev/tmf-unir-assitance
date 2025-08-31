using ProviderService.Domain.Entities;

namespace ProviderService.Domain.Interfaces
{
    public interface IProviderContactRepository
    {
        /// <summary>
        /// Creates a new ProviderContact and stores it in the database.
        /// </summary>
        /// <param name="ProviderContact">The ProviderContact object to be created and stored.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created ProviderContact object, including any database-generated fields like ID.</returns>
        Task<ProviderContact> CreateProviderContactAsync(ProviderContact ProviderContact);

        /// <summary>
        /// Retrieves a ProviderContact by its ID.
        /// </summary>
        /// <param name="idPk">The ID of the Provider to retrieve.</param>
        /// <param name="idSk">The IDClasification of the ProviderContact to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ProviderContact object identified by the given ID, or null if no such object exists.</returns>
        Task<ProviderContact> GetProviderContactByIdAsync(string idPk, string idSk);

        /// <summary>
        /// Retrieves all ProviderContacts stored in the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all stored ProviderContacts.</returns>
        Task<IEnumerable<ProviderContact>> GetAllContactsByPkProviderAsync(string idPk);

        /// <summary>
        /// Updates an existing ProviderContact in the database.
        /// </summary>
        /// <param name="ProviderContact">The ProviderContact object to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateProviderContactAsync(ProviderContact ProviderContact);

        /// <summary>
        /// Deletes a ProviderContact from the database by its ID.
        /// </summary>
        /// <param name="idPk">The ID of the ProviderContact to delete.</param>
        /// <param name="idSk">The ID of the ProviderContact to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the deletion was successful.</returns>
        Task<bool> DeleteProviderContactAsync(string idPk, string idSk);

        /// <summary>
        /// Retrieves Provider data in the database.
        /// </summary>
        /// <param name="Id">The ID of the Provider to retrieve</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ProviderContact object identified by the given ID, or null if no such object exists.</returns>
        Task<ProviderContact> GetProviderMetaDataByIdAsync(string Id);
    }
}
