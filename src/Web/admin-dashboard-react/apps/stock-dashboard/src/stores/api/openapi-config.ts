import type { ConfigFile } from '@rtk-query/codegen-openapi'

const config: ConfigFile = {
  schemaFile: 'http://localhost:32034/swagger/v1/swagger.json',
  apiFile: './stock-api-slice.ts',
  apiImport: 'stockApi',
  outputFile: './generated.ts',
  exportName: 'generated',
  hooks: true,
}

export default config