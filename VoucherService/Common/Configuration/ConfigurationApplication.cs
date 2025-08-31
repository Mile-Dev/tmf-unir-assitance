namespace VoucherService.Common.Configuration
{
    public class ConfigurationApplication(IConfiguration configuration) : IConfigurationApplication
    {
        private readonly IConfiguration configurationApplication = configuration ?? throw new ArgumentNullException(nameof(configuration));

        public string UrlApiSetw => configurationApplication["end_points_settings:url_setw_global"] ?? string.Empty;

        public string PathSetwCoverages => configurationApplication["end_points_settings:path_setw_coverages"] ?? string.Empty;

        public string PasswordSetwCoverages => configurationApplication["end_points_settings:password_setw_global"] ?? string.Empty;
    }
}
