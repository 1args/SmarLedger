using System.Net;

namespace SmartLedger.Hosts.Api.Helpers;

public static class IpAddressHelper
{
    private static readonly string XRealIp = "X-Real-Ip";
    private static readonly string XOriginalForwardedIp = "X-Original-Forwarded-Ip";

    public static IPAddress? GetIpAddress(this HttpRequest httpRequest)
    {
        return TryGetIpAddressFromHeader(httpRequest, XOriginalForwardedIp)
            ?? TryGetIpAddressFromHeader(httpRequest, XRealIp)
            ?? httpRequest.HttpContext.Connection.RemoteIpAddress;
    }

    private static IPAddress? TryGetIpAddressFromHeader(HttpRequest httpRequest, string headerName)
    {
        if(httpRequest.Headers.TryGetValue(headerName, out var value) 
            && IPAddress.TryParse(value, out var ipAddress))
        {
            return ipAddress;
        }

        return null;
    }
}