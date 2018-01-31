const path = require('path');
const webpack = require('webpack');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
    entry: './Frontend/index.js',
    output: {
        path: path.resolve(__dirname, "wwwroot"),
        filename: 'bundle.js'
    },
    module: {
        rules: [
            {
                test: '/\.js$/',
                include: [
                    path.resolve(__dirname, 'Frontend')
                ],
                loader: 'babel-loader',
                options: {
                    presets: ['es2015']
                }
            },
            {
                test: /\.scss$/,
                use: ExtractTextPlugin.extract({
                    fallback: 'style-loader',
                    use: [
                        { loader: 'css-loader' },
                        {
                            loader: 'postcss-loader',
                            options: {
                                plugins: function () {
                                    return [
                                        require('autoprefixer')
                                    ];
                                }
                            }
                        },
                        { loader: 'sass-loader' }
                    ]
                })
            }
        ]
    },
    plugins: [
        new ExtractTextPlugin('style.css'),
        new webpack.ProvidePlugin({
            $: 'jquery/dist/jquery.min.js'
        })
    ]
};