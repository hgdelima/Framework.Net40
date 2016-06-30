using Alcoa.Framework.Common.Properties;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Alcoa.Framework.Common.Enumerator
{
    /// <summary>
    /// Internal Layer identification
    /// </summary>
    public enum Layer
    {
        Default = 0,
        DataAccess = 1,
        Application = 2,
        Presentation = 3
    }

    /// <summary>
    /// Alcoa Production Databases
    /// </summary>
    [DataContract]
    public enum CommonDatabase
    {
        [EnumMember]
        NONE = 0,
        [EnumMember]
        DBALCAP,
        [EnumMember]
        DBALUAP,
        [EnumMember]
        DBITPAP,
        [EnumMember]
        DBITPBP,
        [EnumMember]
        DBPOCAP,
        [EnumMember]
        DBTUBBP,
        [EnumMember]
        DBUTGAP,
        [EnumMember]
        DBUTGBP,
        [EnumMember]
        P300,
        [EnumMember]
        P335,
    }

    /// <summary>
    /// Common translations paramenters for WEB_TRANSLATIONS table (SSO)
    /// </summary>
    public enum CommonTranslationParameter
    {
        [Description("Translation Type")]
        TRANS_TP_EXC_TP_ID,
        [Description("Translation Type of translations type")]
        TRANS_TP_ID_TRANS_TP,
        [Description("Translation Type for Services")]
        TRANS_TP_EXC_SERVICO,
        [Description("Tipo de tradução para serviços")]
        TRANS_SERV_TYPE_ID,
        [Description("Note of Service Translation Type")]
        TRANS_TP_EXC_NT_SERV,
        [Description("Translation TYPE para a observação do serviço")]
        TRANS_OBS_SERV_TP_ID,
        [Description("Language type para o apelido do serviço")]
        TRANS_NIK_SERV_TP_ID,
        [Description("Translation Type for Nick Name of Services")]
        TRANS_TP_EXC_NK_SERV,
        [Description("Translation Type for Description of Group of Services")]
        TRANS_TP_EXC_GRUPO,
        [Description("Translation Type para Descrição do Grupo de Serviços")]
        TRANS_GRP_DS_TYPE_ID,
        [Description("Translation TYPE para a descrição da lingua")]
        TRANS_TRADEFAL_TP_ID,
        [Description("Código Translation Type para menssagens")]
        TRANSLATION_TYPE_MSG,
        [Description("Translation Type for Text of Applications")]
        TRANS_TP_EXC_TXT_APP,
        [Description("Tipo de tradução para Aplicação a Observação da Aplicação")]
        TRANS_APLIC_TX_TP_ID,
        [Description("Translation TYPE para a descrição da aplicação")]
        TRANS_APLIC_DS_TP_ID,
        [Description("Translation Type for Application description")]
        TRANS_TP_EXC_APPL,
        [Description("Translation Type para a mascara dos campos data")]
        TRANS_MASK_DT_TP_ID,
        [Description("Translation Type para a mascara do Last Access")]
        TRANS_MASK_LA_TP_ID,
        [Description("Translation Type for Language Description")]
        TRANS_TP_EXC_LANG,
        [Description("Translation TYPE para o Hint da mascara de data")]
        TRANS_HINTMASK_TP_ID,
        [Description("Translation Type for Lable")]
        TRANS_TP_LABLE,
        [Description("Translation Type for Contents of Variable")]
        TRANS_CONT_VAR_TP_ID,
        [Description("Translation Type para Nome de Variavel")]
        TRANS_TP_EXC_NVAR,
        [Description("Translation Type para Descrição de Variavel")]
        TRANS_TP_EXC_DVAR,
        [Description("Translation Type for Description of Variable")]
        TRANS_DESC_VAR_TP_ID,
        [Description("Translation Type para Grupo de Exibição")]
        TRANS_TP_EXC_GRP_EXB,
        [Description("Tipo de tradução para Grupo de Exibição de Serviço")]
        TRANS_GRP_EX_TYPE_ID
    }

    /// <summary>
    /// Alcoa common AppSettings keys names, for all applications
    /// </summary>
    public enum CommonAppSettingKeyName
    {
        ApplicationCode,
        ApplicationGarInstanceId,
        ApplicationGarUsageId,
        DomainActiveDirectory,
        DefaultUnhandledExceptionLevel,
        EnableMailSend,
        EmailDefaultTo,
        EmailDefaultFrom,
        ImpersonationUserDomain,
        ImpersonationUserLogin,
        ImpersonationUserPass,
        SmtpServer,
        SmtpServerIsSsl,
        SmtpServerPortNumber,
        SmtpUser,
        SmtpPassword,
        SmtpTimeout,
        ServiceEntryErrorLogLevel,
        ServiceTimeInterval,
        SsoApplicationCode,
    }

    /// <summary>
    /// Framework Exception Types for all applications
    /// </summary>
    public enum CommonExceptionType
    {
        AppInitializationException,
        AppSettingKeyNotFound,
        InvalidUsername,
        InvalidPassword,
        InvalidUsernameAndPassword,
        ParameterException,
        SerializationException,
        SsoInitializationException,
        ServiceEndpointNotFound,
        UnknowException,
        ValidationException,
    }

    /// <summary>
    /// Log levels
    /// </summary>
    public enum LogLevel
    {
        Trace = 1, 
        Debug = 2,
        Info = 3,
        Warn = 4,
        Error = 5,
        Fatal = 6,
        Off = 7
    }

    /// <summary>
    /// SSO WebSignature RSA Keys
    /// </summary>
    public enum WebSignatureRsaKey
    {
        [Description("")]
        PrivateKey,
        [Description("")]
        PublicKey
    }

    /// <summary>
    /// Specifies the type of login used.
    /// </summary>
    public enum LogonType
    {
        /// <summary>
        /// This logon type is intended for users who will be interactively using the computer, such as a user being logged
        /// on by a terminal server, remote shell, or similar process. This logon type has the additional expense of caching
        /// logon information for disconnected operations; therefore, it is inappropriate for some client/server applications,
        /// such as a mail server.
        /// </summary>
        Interactive = 2,

        /// <summary>
        /// This logon type is intended for high performance servers to authenticate plaintext passwords.
        /// The LogonUser function does not cache credentials for this logon type.
        /// </summary>
        Network = 3,

        /// <summary>
        /// This logon type is intended for batch servers, where processes may be executing on behalf of a user
        /// without their direct intervention. This type is also for higher performance servers that process many
        /// plaintext authentication attempts at a time, such as mail or web servers.
        /// </summary>
        Batch = 4,

        /// <summary>
        /// Indicates a service-type logon. The account provided must have the service privilege enabled. 
        /// </summary>
        Service = 5,

        /// <summary>
        /// GINAs are no longer supported.
        /// Windows Server 2003 and Windows XP:  This logon type is for GINA DLLs that log on users who will be
        /// interactively using the computer. This logon type can generate a unique audit record that shows when
        /// the workstation was unlocked.
        /// </summary>
        Unlock = 7,

        /// <summary>
        /// This logon type preserves the name and password in the authentication package, which allows the server
        /// to make connections to other network servers while impersonating the client. A server can accept plaintext
        /// credentials from a client, call LogonUser, verify that the user can access the system across the network,
        /// and still communicate with other servers.
        /// </summary>
        NetworkCleartext = 8,

        /// <summary>
        /// This logon type allows the caller to clone its current token and specify new credentials for outbound connections.
        /// The new logon session has the same local identifier but uses different credentials for other network connections.
        /// This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
        /// </summary>
        NewCredentials = 9,
    }

    /// <summary>
    /// Get resources from Common assembly
    /// </summary>
    public static class CommonResource
    {
        /// <summary>
        /// Get content at Assembly Resource using a key
        /// </summary>
        public static string GetString(string key)
        {
            return Resources.ResourceManager.GetString(key) ?? string.Empty;
        }
    }
}