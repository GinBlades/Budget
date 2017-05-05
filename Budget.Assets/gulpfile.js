var gulp = require('gulp'),
    webpack = require('webpack'),
    plumber = require("gulp-plumber"),
    sourceMaps = require("gulp-sourcemaps"),
    tsc = require("gulp-typescript"),
    tsLint = require("gulp-tslint"),
    concat = require("gulp-concat"),
    sass = require("gulp-sass"),
    uglify = require("gulp-uglify"),
    gulpWebpack = require("gulp-webpack");


var styleEP = ["app/main.scss"];
var scriptEP = ["app/**/*.ts"];
var appDir = "../Budget.Web/wwwroot/";

gulp.task("pack", function () {
    return gulp.src(scriptEP)
        .pipe(gulpWebpack({
            output: {
                filename: "main.js"
            },
            module: {
                loaders: [
                    { test: /\.tsx?$/, loader: "ts-loader" }
                ]
            },
            resolve: {
                extensions: [".ts", ".tsx", ".js", ""]
            },
            plugins: [
                new webpack.ProvidePlugin({
                    $: "jquery",
                    jQuery: "jquery"
                })
            ]
        }))
        // .pipe(uglify())
        .pipe(gulp.dest(appDir + "js"))
});

gulp.task("copySourceMaps", function () {
    gulp.src([
        "node_modules/jquery/dist/jquery.min.map"
    ])
        .pipe(gulp.dest(appDir + "js"));
});

gulp.task("copyFonts", function () {
    return gulp.src("node_modules/bootstrap-sass/assets/fonts/bootstrap/*.*")
        .pipe(gulp.dest(appDir + "fonts"));
});

gulp.task("sass", function () {
    return gulp.src(styleEP)
        .pipe(plumber())
        .pipe(sourceMaps.init())
        .pipe(sass({ style: "compressed" }))
        .pipe(sourceMaps.write("."))
        .pipe(gulp.dest(appDir + "css"));
});

gulp.task("watch", function () {
    gulp.watch(styles, ["sass"]);
    gulp.watch(scripts, ["pack"]);
});

gulp.task("build", ["copyFonts", "sass", "pack"]);

gulp.task('default', ["build", "watch"]);