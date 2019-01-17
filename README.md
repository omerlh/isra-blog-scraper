# Isra Blog Scraper
A smal utility that can download a whole blog from Isra Blog.

## Usage

```
isra-blog-scraper -y <last-active-year> -b <blog-id>
```
You can get the blog id from your blog URL:
```
http://israblog.nana10.co.il/blogread.asp?blog=<blog-id>
```
After running, all the pages with posts on your blog will be under `blog` folder, orderd by year and months. Months without posts will results in empty folders.

## Options

```
Copyright (C) 2019 isra-blog-scraper

  -y, --year               Required. last post published year

  -b, --blogId             Required. the ID of the blog

  -o, --outputDirectory    the output directory for the blog results

  --help                   Display this help screen.

  --version                Display version information.
```