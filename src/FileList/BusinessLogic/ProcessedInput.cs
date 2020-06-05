using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace JsonTaggerApi.FileList.BusinessLogic
{
    public class ProcessedInput
    {
        public ImmutableList<string> IncludeTerms { get; }
        public ImmutableList<string> ExcludeTerms { get; }
        public int Page { get; }

        public ProcessedInput(string? userSearchInput, string? page)
        {
            IEnumerable<string> split = userSearchInput
                ?.Split(' ')
                ?.Where(x => !String.IsNullOrWhiteSpace(x))
                ?? new List<string>();
            
            IncludeTerms = split
                .Where(x => !x.StartsWith('-'))
                .ToImmutableList();

            ExcludeTerms = split
                .Where(x => x.StartsWith('-'))
                .Select(x => x.Substring(1))
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .ToImmutableList();

            Page = Utility.ParseIntDefaultZero(page);
        } 
    }
}
