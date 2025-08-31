using ProviderService.Domain.Entities;

namespace ProviderService.Domain.Interfaces
{
    public interface IProviderLocationRepository
    {
        /// <summary>
        /// Creates a new ProviderLocation and stores it in the database.
        /// </summary>
        /// <param name="providerLocation">The ProviderLocation object to be created and stored.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created ProviderLocation object, including any database-generated fields like ID.</returns>
        Task<ProviderLocation> CreateProviderLocationAsync(ProviderLocation providerLocation);

        /// <summary>
        /// Retrieves a ProviderLocation by its ID.
        /// </summary>
        /// <param name="idPk">The ID of the Provider to retrieve.</param>
        /// <param name="idSk">The ID of the ProviderLocation to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ProviderLocation object identified by the given ID, or null if no such object exists.</returns>
        Task<ProviderLocation> GetProviderLocationByIdAsync(string idPk, string idSk);

        /// <summary>
        /// Retrieves all ProviderLocations stored in the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all stored ProviderLocations.</returns>
        Task<IEnumerable<ProviderLocation>> GetAllLocationsByProviderAsync(string idPk);

        /// <summary>
        /// Updates an existing ProviderLocation in the database.
        /// </summary>
        /// <param name="providerLocation">The ProviderLocation object to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateProviderLocationAsync(ProviderLocation providerLocation);

        /// <summary>
        /// Deletes a ProviderLocation from the database by its ID.
        /// </summary>
        /// <param name="idPk">The ID of the Provider to delete.</param>
        /// <param name="idSk">The ID clasification of the ProviderLocation to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the deletion was successful.</returns>
        Task<bool> DeleteProviderLocationAsync(string idPk, string idSk);

        /// <summary>
        /// Retrieves Provider data in the database.
        /// </summary>
        /// <param name="Id">The ID of the Provider to retrieve</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ProviderAgreement object identified by the given ID, or null if no such object exists.</returns>
        Task<ProviderLocation> GetProviderMetaDataByIdAsync(string Id);
    }
}
