﻿using Microsoft.Html.Editor.Completion;
using Microsoft.Html.Editor.Completion.Def;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Utilities;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Glyphfriend
{
    [HtmlCompletionProvider(CompletionTypes.Values, "*", "class")]
    [ContentType("htmlx")]
    internal class GlyphCompletionListProvider : BaseHtmlCompletionListProvider
    {
        [Import]
        protected SVsServiceProvider GlobalServiceProvider { get; private set; }

        public override string CompletionType { get { return CompletionTypes.Values; } }

        public override IList<HtmlCompletion> GetEntries(HtmlCompletionContext context)
        {
            VSPackage package = (VSPackage)EnsurePackageLoaded();
            if (package == null)
            {
                Logger.Log("Package failed to load properly!");
                return new List<HtmlCompletion>();
            }

            var glyphCompletionItems = new List<HtmlCompletion>();

            // Get the filtered set of enabled glyphs
            var enabledGlyphs = package.Glyphs.Where(g => g.Enabled);
            foreach (var glyph in enabledGlyphs)
            {
                glyphCompletionItems.Add(CreateItem(glyph.Name, glyph.Image, context.Session));
            }

            return glyphCompletionItems;
        }

        private IVsPackage EnsurePackageLoaded()
        {
            return GlobalServiceProvider.GetShell().LoadPackage<VSPackage>();
        }
    }
}