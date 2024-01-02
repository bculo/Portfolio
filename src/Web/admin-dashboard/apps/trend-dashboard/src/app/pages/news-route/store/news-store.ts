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
import { addEntities, setAllEntities, withEntities } from '@ngrx/signals/entities';
import { Article } from '../models/news.model';
import { NewsService } from '../../../shared/services/open-api';
import { mapToArticleArray } from '../mappers/mapper';

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
        fetchLatest: rxMethod<void>(
            pipe(
                switchMap(() =>
                    service.getLatestNews().pipe(
                        map(mapToArticleArray),
                        tapResponse({
                            next: (response) => patchState(store, setAllEntities(response)),
                            error: console.error,
                            finalize: () => patchState(store, { isLoading: false }),
                        }),
                    )
                ),
            ),
        ),
    })),
    withHooks({
        onInit({fetchLatest}) {
            fetchLatest()
        }
    }),
);