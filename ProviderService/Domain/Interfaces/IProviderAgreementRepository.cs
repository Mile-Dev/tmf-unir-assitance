using ProviderService.Domain.Entities;

namespace ProviderService.Domain.Interfaces
{
    public interface IProviderAgreementRepository
    {
        /// <summary>
        /// Creates a new ProviderAgreement and stores it in the database.
        /// </summary>
        /// <param name="ProviderAgreement">The ProviderAgreement object to be created and stored.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created ProviderAgreement object, including any database-generated fields like ID.</returns>
        Task<ProviderAgreement> CreateProviderAgreementAsync(ProviderAgreement ProviderAgreement);

        /// <summary>
        /// Retrieves a ProviderAgreement by its ID.
        /// </summary>
        /// <param name="idPk">The ID of the Provider to retrieve.</param>
        /// <param name="idSk">The ID of clasification of the Provider to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ProviderAgreement object identified by the given ID, or null if no such object exists.</returns>
        Task<ProviderAgreement> GetProviderAgreementByIdAsync(string idPk, string idSk);

        /// <summary>
        /// Retrieves all ProviderAgreements stored in the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all stored ProviderAgreements.</returns>
        Task<IEnumerable<ProviderAgreement>> GetAllProviderAgreementsAsync(string idPk);

        /// <summary>
        /// Updates an existing ProviderAgreement in the database.
        /// </summary>
        /// <param name="ProviderAgreement">The ProviderAgreement object to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateProviderAgreementAsync(ProviderAgreement ProviderAgreement);

        /// <summary>
        /// Deletes a ProviderAgreement from the database by its ID.
        /// </summary>
        /// <param name="idPk">The ID of the ProviderAgreement to delete.</param
        /// <param name="idSk">The ID Clasification of the ProviderAgreement to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the deletion was successful.</returns>
        Task<bool> DeleteProviderAgreementAsync(string idPk, string idSk);

        /// <summary>
        /// Retrieves Provider data in the database.
        /// </summary>
        /// <param name="Id">The ID of the Provider to retrieve</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ProviderAgreement object identified by the given ID, or null if no such object exists.</returns>
        Task<ProviderAgreement> GetProviderMetaDataByIdAsync(string Id);
    }
}
