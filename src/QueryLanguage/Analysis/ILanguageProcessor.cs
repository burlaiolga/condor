using QueryLanguage.Models;
using System.Collections.Generic;

namespace QueryLanguage.Analysis
{
    public interface ILanguageProcessor
    {
        List<Country> Find(List<Country> countries, string expression);
    }
}