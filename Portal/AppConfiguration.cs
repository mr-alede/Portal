namespace Portal
{
    public class AppConfiguration
    {
        public class Authentication
        {
            public string SecretKey { get; set; }

            public ExternalAuthProvider Twitter { get; set; }
            public ExternalAuthProvider GitHub { get; set; }

            /// <summary>
            /// Gets or sets Facebook auth properties
            /// </summary>
            /// <returns>Facebook auth properties</returns>
            public ExternalAuthProvider Facebook { get; set; }

            /// <summary>
            /// Gets or sets Google auth properties
            /// </summary>
            /// <returns>Google auth properties</returns>
            public ExternalAuthProvider Google { get; set; }

            /// <summary>
            /// Gets or sets LinkedIn auth properties
            /// </summary>
            /// <returns>LinkedIn auth properties</returns>
            public ExternalAuthProvider LinkedIn { get; set; }

            /// <summary>
            /// Gets or sets AzureAD auth properties
            /// </summary>
            /// <returns>AzureAD auth properties</returns>
            public AzureAdProvider AzureAD { get; set; }
        }

        /// <summary>
        /// External authentication configuration section
        /// </summary>
        public class ExternalAuthProvider
        {
            /// <summary>
            /// Gets or sets a value indicating whether External auth provider is enabled
            /// </summary>
            /// <returns>Enabled status</returns>
            public bool Enabled { get; set; }

            /// <summary>
            /// Gets or sets Application key
            /// </summary>
            /// <returns>Application key</returns>
            public string AppKey { get; set; }

            /// <summary>
            /// Gets or sets Application secret
            /// </summary>
            /// <returns>Application secret</returns>
            public string AppSecret { get; set; }
        }

        /// <summary>
        /// External authentication configuration section
        /// </summary>
        public class AzureAdProvider : ExternalAuthProvider
        {
            /// <summary>
            /// Gets or sets Azure tenant ID
            /// </summary>
            /// <returns>Azure tenant ID</returns>
            public string TenantId { get; set; }
        }
    }
}
