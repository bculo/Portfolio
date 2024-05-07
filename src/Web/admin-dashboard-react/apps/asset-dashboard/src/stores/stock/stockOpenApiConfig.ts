import type { ConfigFile } from '@rtk-query/codegen-openapi';

const config: ConfigFile = {
  schemaFile: 'http://localhost:32034/swagger/v1/swagger.json',
  apiFile: './stockApiSlice.ts',
  apiImport: 'stockApi',
  outputFile: './stockApiGenerated.ts',
  exportName: 'stockApiGenerated',
  hooks: {
    lazyQueries: true,
    mutations: true,
    queries: true,
  },
  flattenArg: true,
};

export default config;
