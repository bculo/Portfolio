export * from './news.service';
import { NewsService } from './news.service';
export * from './search-word.service';
import { SearchWordService } from './search-word.service';
export * from './sync.service';
import { SyncService } from './sync.service';
export const APIS = [NewsService, SearchWordService, SyncService];
