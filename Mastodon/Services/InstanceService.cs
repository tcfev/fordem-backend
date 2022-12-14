using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Mastodon.Client;
using Mastodon.Grpc;
using Microsoft.AspNetCore.Http.Extensions;

namespace Mastodon.Services;

public sealed class InstanceService : Mastodon.Grpc.InstanceApi.InstanceApiBase
{
    private readonly MastodonClient _mastodon;
    private readonly ILogger<InstanceService> _logger;
    public InstanceService(ILogger<InstanceService> logger, MastodonClient mastodon)
    {
        _logger = logger;
        _mastodon = mastodon;
    }

    public override async Task<Grpc.InstanceV1> GetInstanceV1(Empty request, ServerCallContext context)
    {
        _mastodon.SetDefaults(context);

        var result = await _mastodon.Instance.GetInstanceV1Async();
        result.RaiseExceptions();

        await result.WriteHeadersTo(context);

        result.Data!.Uri = WebFingerHelper.FixUrl(result.Data!.Uri, context.GetHttpContext().Request.GetEncodedUrl());

        return result.Data!.ToGrpc();
    }

    public override async Task<Grpc.Instance> GetInstance(Empty request, ServerCallContext context)
    {
        var instance = (await _mastodon.Instance.GetInstanceAsync())!;

        instance.Domain = context.GetHttpContext().Request.Host.Value;

        return instance.ToGrpc();
    }

    public override async Task<Activities> GetActivities(Empty request, ServerCallContext context)
    {
        var result = (await _mastodon.Instance.GetActivitiesAsync());
        return result.ToGrpc();
    }

    public override async Task<Rules> GetRules(Empty request, ServerCallContext context)
    {
        var result = (await _mastodon.Instance.GetRulesAsync());
        return result.ToGrpc();
    }

    public override async Task<Lists> GetLists(Empty request, ServerCallContext context)
    {
        _mastodon.SetDefaults(context);

        var result = (await _mastodon.Lists.GetListsAsync());
        return result.ToGrpc();
    }
}