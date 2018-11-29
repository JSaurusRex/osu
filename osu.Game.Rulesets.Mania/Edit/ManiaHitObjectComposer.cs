﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Mania.Objects;
using osu.Game.Rulesets.Mania.Objects.Drawables;
using osu.Game.Rulesets.Objects.Drawables;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Mania.Edit.Blueprints;
using osu.Game.Rulesets.Mania.UI;
using osu.Game.Rulesets.UI;
using osuTK;

namespace osu.Game.Rulesets.Mania.Edit
{
    [Cached(Type = typeof(IManiaHitObjectComposer))]
    public class ManiaHitObjectComposer : HitObjectComposer<ManiaHitObject>, IManiaHitObjectComposer
    {
        protected new ManiaEditRulesetContainer RulesetContainer { get; private set; }

        public ManiaHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
        {
        }

        /// <summary>
        /// Retrieves the column that intersects a screen-space position.
        /// </summary>
        /// <param name="screenSpacePosition">The screen-space position.</param>
        /// <returns>The column which intersects with <paramref name="screenSpacePosition"/>.</returns>
        public Column ColumnAt(Vector2 screenSpacePosition) => RulesetContainer.GetColumnByPosition(screenSpacePosition);

        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        protected override RulesetContainer<ManiaHitObject> CreateRulesetContainer(Ruleset ruleset, WorkingBeatmap beatmap)
        {
            RulesetContainer = new ManiaEditRulesetContainer(ruleset, beatmap);

            // This is the earliest we can cache the scrolling info to ourselves, before masks are added to the hierarchy and inject it
            dependencies.CacheAs(RulesetContainer.ScrollingInfo);

            return RulesetContainer;
        }

        protected override IReadOnlyList<HitObjectCompositionTool> CompositionTools => new HitObjectCompositionTool[]
        {
            new NoteCompositionTool()
        };

        public override SelectionBlueprint CreateBlueprintFor(DrawableHitObject hitObject)
        {
            switch (hitObject)
            {
                case DrawableNote note:
                    return new NoteSelectionBlueprint(note);
                case DrawableHoldNote holdNote:
                    return new HoldNoteSelectionBlueprint(holdNote);
            }

            return base.CreateBlueprintFor(hitObject);
        }
    }
}
