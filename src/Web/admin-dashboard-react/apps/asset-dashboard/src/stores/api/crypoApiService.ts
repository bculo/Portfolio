import { cryptoApiGenerated } from './cryptoApiGenerated';

export const apiService = cryptoApiGenerated.enhanceEndpoints({
  addTagTypes: ['Crypto'],
});
