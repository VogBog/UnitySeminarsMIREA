using Base;

namespace MultiNetwork
{
    public interface INetworkTypeRequirer
    {
        StaticParameters.NetworkTypes RequiredNetworkType { get; }
    }
}