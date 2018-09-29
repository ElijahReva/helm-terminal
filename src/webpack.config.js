const path = require('path');
const webpack = require('webpack');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const VueLoaderPlugin = require('vue-loader/lib/plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');

const bundleOutputDir = './wwwroot/dist/';

module.exports = {
  resolve: {
    extensions: ['*', '.js', '.vue', '.json'],
    alias: {
      vue$: 'vue/dist/vue.esm.js',
      // eslint-disable-next-line no-undef
      '../../theme.config$': path.join(__dirname, 'semantic-theme/theme.config')
    }
  },
  module: {
    rules: [
      {
        test: /\.vue$/,
        loader: 'vue-loader',
        options: {
          loaders: {
            less: 'vue-style-loader!css-loader!less-loader'
          }
        }
      },
      {
        test: /\.js$/,
        loader: 'babel-loader',
        exclude: /node_modules/
      },

      {
        test: /\.less$/,
        use: [
          { loader: 'vue-style-loader' },
          { loader: 'url-loader' },
          { loader: 'css-loader' },
          { loader: 'less-loader' }
        ]
      },
      {
        test: /\.css$/,
        use: ['style-loader', 'css-loader']
      },
      {
        test: /\.json$/,
        use: [{ loader: 'json-loader' }]
      },
      {
        test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/,
        loader: 'url-loader?limit=10000&mimetype=application/fontwoff'
      },
      {
        test: /\.jpe?g$|\.gif$|\.png$|\.ttf$|\.eot$|\.svg$/,
        use: 'file-loader?name=[name].[ext]?[hash]'
      }
    ]
  },
  entry: { main: './Frontend/app.js' },
  output: {
    filename: '[name].js',
    // eslint-disable-next-line no-undef
    path: path.join(__dirname, bundleOutputDir),
    publicPath: 'dist/'
  },
  mode: 'development',
  devtool: 'inline-source-map',
  devServer: {
    contentBase: bundleOutputDir,
    historyApiFallback: true
  },
  plugins: [
    new VueLoaderPlugin(),
    new  HtmlWebpackPlugin({
        hash: true,
        filename: '../index.html',
        title: 'Helm Terminal'
    }),
    // eslint-disable-next-line no-undef
    new webpack.ProvidePlugin({
      $: 'jquery',
      jQuery: 'jquery',
      'windows.jQuery': 'jquery'
    }),
    new CopyWebpackPlugin([
      {
        from: './Resources/*',
        to: './',
        flatten: true
      }
    ])
  ]
};
