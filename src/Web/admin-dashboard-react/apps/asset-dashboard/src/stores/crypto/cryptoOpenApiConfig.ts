import type { ConfigFile } from '@rtk-query/codegen-openapi';

const config: ConfigFile = {
  schemaFile: 'http://localhost:5263/swagger/v1/swagger.json',
  apiFile: './cryptoApiSlice.ts',
  apiImport: 'cryptoApi',
  outputFile: './cryptoApiGenerated.ts',
  exportName: 'cryptoApiGenerated',
  hooks: {
    lazyQueries: true,
    mutations: true,
    queries: true,
  },
  flattenArg: true,
};

export default config;
