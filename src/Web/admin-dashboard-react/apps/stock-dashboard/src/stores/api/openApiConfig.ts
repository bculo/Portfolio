import type { ConfigFile } from '@rtk-query/codegen-openapi'

const config: ConfigFile = {
  schemaFile: 'http://localhost:32034/swagger/v1/swagger.json',
  apiFile: './stockApiSlice.ts',
  apiImport: 'stockApi',
  outputFile: './generated.ts',
  exportName: 'generated',
  hooks: {
    lazyQueries: true,
    mutations: true,
    queries: true
  },
  flattenArg: false,
}

export default config