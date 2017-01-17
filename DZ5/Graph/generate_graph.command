#!/bin/sh

OUTPUT_FOLDER="$1"
TIME="$2" # todo check if starts with ...

BASEDIR=$(dirname $0)

$BASEDIR/phantomjs/bin/phantomjs $BASEDIR/generate_graph.js /dir:$BASEDIR /out:$OUTPUT_FOLDER $TIME