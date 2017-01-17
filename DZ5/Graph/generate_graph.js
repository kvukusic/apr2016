// 1st argument - base directory (path from which the script was called)
// 2st argument - output folder - if empty the current folder is used
// 3nd argument - data

function startsWith(str, word) {
    if (str == undefined) return false;
    return str.lastIndexOf(word, 0) === 0;
}

function endsWith(str, word) {
    if (str == undefined) return false;
    return str.lastIndexOf(word) === str.length - 1;
}

var system = require('system');
var args = system.args;

var baseDir = './';
var folder = '';
var graphData = '{}';

// args.forEach(function(arg, i) {
//     console.log(i + ": " + arg);
// });

args.forEach(function(arg, i) {
    if (startsWith(arg, "/dir:")) {
        baseDir = arg.replace("/dir:", "");
        if (!endsWith(baseDir, "/")) 
            baseDir = baseDir + '/';
    } else if (startsWith(arg, "/out:")) {
        folder = arg.replace("/out:", "");
        if (!endsWith(folder, "/"))
            folder = folder + '/';
    } else if (startsWith(arg, "/data:")) {
        graphData = arg.replace("/data:", "");
    }
});

var graph = JSON.parse(graphData);

// Generate html content
var expectedContent = '';

expectedContent += "<!DOCTYPE html> <meta charset=\"utf-8\"> <html lang=\"en\"> <head> <style> body { font: 10px sans-serif; background: #FFFFFF } .axis path, .axis line { fill: none; stroke: #000; shape-rendering: crispEdges; } .grid path, .grid line { fill: none; stroke: rgba(0, 0, 0, 0.15); shape-rendering: crispEdges; } .x.axis path { display: none; } .line { fill: none; stroke-width: 1.5px; } <\/style> <\/head> <body> <div>";

graph.graphs.forEach(function(g, i) {
    var variableName = "Var" + (i + 1);

    expectedContent += "<div> <script src=\"d3\/d3.v3.min.js\" charset=\"utf-8\"><\/script> <script>";
    expectedContent += "var data = ";

    var data = []

    g.lines.forEach(function(line, j) {
        var obj = {};

        var lineLabel = line.label;
        
        var xs = line.items.map(function (item) {
            return item.time;
        })
        var ys = line.items.map(function (item) {
            return item.variableValue;
        });

        obj["label"] = lineLabel;
        obj["x"] = xs;
        obj["y"] = ys;

        data.push(obj);
    });

    expectedContent += JSON.stringify(data);
    expectedContent += ";";
    expectedContent += "var xy_chart = d3_xy_chart() .width(1000) .height(500) .xlabel(\"t\") .ylabel(\"";
    expectedContent += variableName;
    expectedContent += "\");";

    expectedContent += "var svg = d3.select(\"body\").append(\"svg\") .datum(data) .call(xy_chart); function d3_xy_chart() { var width = 640, height = 480, xlabel = \"X Axis Label\", ylabel = \"Y Axis Label\"; function chart(selection) { selection.each(function (datasets) { var margin = { top: 30, right: 100, bottom: 30, left: 100 }, innerwidth = width - margin.left - margin.right, innerheight = height - margin.top - margin.bottom; var x_scale = d3.scale.linear() .range([0, innerwidth]) .domain([d3.min(datasets, function (d) { return d3.min(d.x); }), d3.max(datasets, function (d) { return d3.max(d.x); })]); var y_scale = d3.scale.linear() .range([innerheight, 0]) .domain([d3.min(datasets, function (d) { return d3.min(d.y); }), d3.max(datasets, function (d) { return d3.max(d.y); })]); var color_scale = d3.scale.category10() .domain(d3.range(datasets.length)); var x_axis = d3.svg.axis() .scale(x_scale) .orient(\"bottom\"); var y_axis = d3.svg.axis() .scale(y_scale) .orient(\"left\"); var x_grid = d3.svg.axis() .scale(x_scale) .orient(\"bottom\") .tickSize(-innerheight) .tickFormat(\"\"); var y_grid = d3.svg.axis() .scale(y_scale) .orient(\"left\") .tickSize(-innerwidth) .tickFormat(\"\"); var draw_line = d3.svg.line() .interpolate(\"basis\") .x(function (d) { return x_scale(d[0]); }) .y(function (d) { return y_scale(d[1]); }); var svg = d3.select(this) .attr(\"width\", width) .attr(\"height\", height) .append(\"g\") .attr(\"transform\", \"translate(\" + margin.left + \",\" + margin.top + \")\"); svg.append(\"g\") .attr(\"class\", \"x grid\") .attr(\"transform\", \"translate(0,\" + innerheight + \")\") .call(x_grid); svg.append(\"g\") .attr(\"class\", \"y grid\") .call(y_grid); svg.append(\"g\") .attr(\"class\", \"x axis\") .attr(\"transform\", \"translate(0,\" + innerheight + \")\") .call(x_axis) .append(\"text\") .attr(\"dy\", \"-.71em\") .attr(\"x\", innerwidth) .style(\"text-anchor\", \"end\") .text(xlabel); svg.append(\"g\") .attr(\"class\", \"y axis\") .call(y_axis) .append(\"text\") .attr(\"transform\", \"rotate(-90)\") .attr(\"y\", 6) .attr(\"dy\", \"0.71em\") .style(\"text-anchor\", \"end\") .text(ylabel); var data_lines = svg.selectAll(\".d3_xy_chart_line\") .data(datasets.map(function (d) { return d3.zip(d.x, d.y); })) .enter().append(\"g\") .attr(\"class\", \"d3_xy_chart_line\"); data_lines.append(\"path\") .attr(\"class\", \"line\") .attr(\"d\", function (d) { return draw_line(d); }) .attr(\"stroke\", function (_, i) { return color_scale(i); }); data_lines.append(\"text\") .datum(function (d, i) { return { name: datasets[i].label, final: d[d.length - 1] }; }) .attr(\"transform\", function (d) { return (\"translate(\" + x_scale(d.final[0]) + \",\" + y_scale(d.final[1]) + \")\"); }) .attr(\"x\", 3) .attr(\"dy\", \".35em\") .attr(\"fill\", function (_, i) { return color_scale(i); }) .text(function (d) { return d.name; }); }); } chart.width = function (value) { if (!arguments.length) return width; width = value; return chart; }; chart.height = function (value) { if (!arguments.length) return height; height = value; return chart; }; chart.xlabel = function (value) { if (!arguments.length) return xlabel; xlabel = value; return chart; }; chart.ylabel = function (value) { if (!arguments.length) return ylabel; ylabel = value; return chart; }; return chart; } <\/script> <\/div>";
});

expectedContent += "<\/div> <\/body> <\/html>";

var timestamp = Math.floor(Date.now() / 1000);

var webPage = require('webpage');
var page = webPage.create();
var expectedLocation = "file://" + baseDir;

var fs = require('fs');
var originalWordkingDirectory = fs.workingDirectory;
// fs.changeWorkingDirectory(baseDir);
// console.log(fs.workingDirectory);
// console.log(baseDir);
// console.log(folder);

page.setContent(expectedContent, expectedLocation);

page.onLoadFinished = function() {
    // fs.changeWorkingDirectory(originalWordkingDirectory);
    // fs.write(folder + 'result_' + timestamp + '.html', page.content, 'w');
    page.render(folder + 'result_' + timestamp + '.png');
    phantom.exit();

    // page = webPage.create();
    // page.open(folder + 'result_' + timestamp + '.html')
    // page.onLoadFinished = function () {
    //     page.render(folder + 'result_' + timestamp + '.png');
    //     phantom.exit();
    // }
}
