namespace Crypto.Application.Interfaces.Repositories.Models;

public record TimeFrameQuery(int NotOlderThanMin, int TimeBucketMin);