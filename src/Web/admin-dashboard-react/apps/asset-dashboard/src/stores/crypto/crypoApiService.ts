import { cryptoApiGenerated } from './cryptoApiGenerated';

export const cryptoApiService = cryptoApiGenerated.enhanceEndpoints({
  addTagTypes: ['Crypto'],
});
