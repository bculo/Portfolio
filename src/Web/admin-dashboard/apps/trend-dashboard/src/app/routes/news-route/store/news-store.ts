import {
    signalStore,
    patchState,
    withMethods,
    withState,
    withHooks,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { inject } from '@angular/core';
import { map, pipe, switchMap, tap } from 'rxjs';
import { tapResponse } from '@ngrx/operators';
import { withDevtools } from '@angular-architects/ngrx-toolkit'
import { addEntities, removeEntity, setAllEntities, withEntities } from '@ngrx/signals/entities';
import { Article } from '../models/news.model';
import { NewsService } from '../../../shared/services/open-api';
import { mapToArticleArray } from '../mappers/mapper';
import { ActiveEnumOptions, ContextTypeEnumOptions } from '../../../shared/enums/enums';
import { NewsFilterModel } from './news-store.model';

interface NewsState {
    isLoading: boolean
}

const initialState: NewsState = {
    isLoading: true,
}

export const NewsStore = signalStore(
    { providedIn: 'root' },
    withState(initialState),
    withEntities<Article>(),
    withDevtools('news'),
    withMethods((store, service = inject(NewsService)) => ({

        fetch: rxMethod<NewsFilterModel>(
            pipe(
                switchMap((search) =>
                    service.filterNews(ContextTypeEnumOptions.All, ActiveEnumOptions.All, search.query ?? '', 1, 500).pipe(
                        map((res) => mapToArticleArray(res.items!)),
                        tapResponse({
                            next: (response) => patchState(store, setAllEntities(response)),
                            error: console.error,
                            finalize: () => patchState(store, { isLoading: false }),
                        }),
                    )
                ),
            ),
        ),

        deactivate: rxMethod<string>(
            pipe(
                tap(() => patchState(store, { isLoading: false })),
                switchMap((articleId) =>
                    service.deactivateArticle(articleId).pipe(
                        tapResponse({
                            next: (response) => patchState(store, removeEntity(articleId)),
                            error: console.error,
                            finalize: () => patchState(store, { isLoading: false }),
                        }),
                    )
                ),
            ),
        ),

    })),
    withHooks({
        onInit({fetch}) {
            fetch({query: ''})
        }
    }),
);