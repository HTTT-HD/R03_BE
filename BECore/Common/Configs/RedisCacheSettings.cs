namespace Common.Configs
{
    public class RedisCacheSettings
    {
        public bool Enable { get; set; }
        public string ConnectionString { get; set; }
        public bool AbortOnConnectFail { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
