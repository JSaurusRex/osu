﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Beatmaps.Drawables.Cards;
using osu.Game.Online.API;
using osu.Game.Online.API.Requests;
using osu.Game.Online.API.Requests.Responses;
using osuTK;
using APIUser = osu.Game.Online.API.Requests.Responses.APIUser;

namespace osu.Game.Overlays.Profile.Sections.Beatmaps
{
    public class PaginatedBeatmapContainer : PaginatedProfileSubsection<APIBeatmapSet>
    {
        private const float panel_padding = 10f;
        private readonly BeatmapSetType type;

        protected override int InitialItemsCount => type == BeatmapSetType.Graveyard ? 2 : 6;

        public PaginatedBeatmapContainer(BeatmapSetType type, Bindable<APIUser> user, LocalisableString headerText)
            : base(user, headerText)
        {
            this.type = type;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            ItemsContainer.Spacing = new Vector2(panel_padding);
        }

        protected override int GetCount(APIUser user)
        {
            switch (type)
            {
                case BeatmapSetType.Favourite:
                    return user.FavouriteBeatmapsetCount;

                case BeatmapSetType.Graveyard:
                    return user.GraveyardBeatmapsetCount;

                case BeatmapSetType.Loved:
                    return user.LovedBeatmapsetCount;

                case BeatmapSetType.Ranked:
                    return user.RankedBeatmapsetCount;

                case BeatmapSetType.Pending:
                    return user.PendingBeatmapsetCount;

                default:
                    return 0;
            }
        }

        protected override APIRequest<List<APIBeatmapSet>> CreateRequest(PaginationParameters pagination) =>
            new GetUserBeatmapsRequest(User.Value.Id, type, pagination);

        protected override Drawable CreateDrawableItem(APIBeatmapSet model) => model.OnlineID > 0
            ? new BeatmapCardNormal(model)
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
            }
            : null;
    }
}
