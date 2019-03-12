module.exports = {
  lintOnSave: false,
  productionSourceMap: false,
  filenameHashing: false,
  outputDir: '../wwwroot',
  parallel: true,

  devServer: {
    proxy: 'https://localhost:5001'
  },

  baseUrl: undefined,
  assetsDir: undefined,
  runtimeCompiler: undefined,
  css: undefined
}