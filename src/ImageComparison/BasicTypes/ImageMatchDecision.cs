namespace JsonTaggerApi.ImageComparison.BasicTypes
{
    public enum ImageMatchDecision {
        ToDecide,
        KeepFirstOnly,
        KeepSecondOnly,
        KeepBothNoMatch,
        KeepBothSameSet,
        KeepBothOtherReason
    }
}
