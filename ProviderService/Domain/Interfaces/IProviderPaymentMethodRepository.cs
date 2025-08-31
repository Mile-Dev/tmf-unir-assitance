using ProviderService.Domain.Entities;

namespace ProviderService.Domain.Interfaces
{
    public interface IProviderPaymentMethodRepository
    {
        /// <summary>
        /// Creates a new ProviderPaymentMethod and stores it in the database.
        /// </summary>
        /// <param name="ProviderPaymentMethod">The ProviderPaymentMethod object to be created and stored.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created ProviderPaymentMethod object, including any database-generated fields like ID.</returns>
        Task<ProviderPaymentMethod> CreateProviderPaymentMethodAsync(ProviderPaymentMethod ProviderPaymentMethod);

        /// <summary>
        /// Retrieves a ProviderPaymentMethod by its ID.
        /// </summary>
        /// <param name="idPk">The ID of the Provider to retrieve.</param>
        /// <param name="idSk">The IDclasification of the ProviderPaymentMethod to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ProviderPaymentMethod object identified by the given ID, or null if no such object exists.</returns>
        Task<ProviderPaymentMethod> GetProviderPaymentMethodByIdAsync(string idPk, string idSk);

        /// <summary>
        /// Retrieves all ProviderPaymentMethods stored in the database.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all stored ProviderPaymentMethods.</returns>
        Task<IEnumerable<ProviderPaymentMethod>> GetAllProviderPaymentMethodsAsync(string idPk);

        /// <summary>
        /// Updates an existing ProviderPaymentMethod in the database.
        /// </summary>
        /// <param name="ProviderPaymentMethod">The ProviderPaymentMethod object to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the update was successful.</returns>
        Task<bool> UpdateProviderPaymentMethodAsync(ProviderPaymentMethod ProviderPaymentMethod);

        /// <summary>
        /// Deletes a ProviderPaymentMethod from the database by its ID.
        /// </summary>
        /// <param name="idPk">The ID of the ProviderPaymentMethod to delete.</param
        /// <param name="idSk">The ID Clasification of the ProviderPaymentMethod to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the deletion was successful.</returns>
        Task<bool> DeleteProviderPaymentMethodAsync(string idPk, string idSk);

        /// <summary>
        /// Retrieves Provider data in the database.
        /// </summary>
        /// <param name="Id">The ID of the Provider to retrieve</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ProviderPaymentMethod object identified by the given ID, or null if no such object exists.</returns>
        Task<ProviderPaymentMethod> GetProviderMetaDataByIdAsync(string Id);
    }
}
