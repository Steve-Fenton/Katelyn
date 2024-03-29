# Katelyn

Well known for Crawling.

Katelyn is a simple slow-crawler for checking a single root website for crawl errors by following links discovered during the crawl.
The "slow cralwer" is designed to make light demands on your web server, making no concurrent requests. Additional delay can be added 
to make the crawl even slower if necessary.

Note: I'm currently porting the entire application to .Net Core. The console is already available as a .Net Core Console application. 
For the purposes of the migration, the new projects are named Fenton.Katelyn.NameHere. The only project left to move is the Forms UI, 
which will be done next. The old projects will remain until the UI no longer depends on them; then they will be deleted.

## .Net Core

You can run Katelyn from the command line:

    dotnet Katelyn.dll Crawl -address=http://localhost:51746

Or

    dotnet Katelyn.dll Crawl -address=http://localhost:51746 -verbose=false

## Options

Basic Options

 - `-address` is mandatory. Specify the full URI to be used as the root for the crawling session

 - `-verbose` optional, if `true` shows information for every URI crawled

When using only the basic options, the recommended defaults are used for all other settings.
By default, Katelyn crawls all links, scripts, styles, and images that belong to the root
address and ignored any external resources. The crawl is performed one request at a time, with
no additional delay.

If you wish to specify these options manually, you can use the advanced options along with the `CrawlWith` method:

    dotnet Katelyn.dll CrawlWith ...

Advanced Options

 - `-address` is mandatory. Specify the full URI to be used as the root for the crawling session _this should only be websites you own_

 - `-verbose` optional, if `true` shows information for every URI crawled

 - `-includeLinks` Whether to include the "href" attribute from _a_ tags

 - `-includeScripts` Whether to include the "src" attribute from _script_ tags

 - `-includeStyles` Whether to include the "href" attribute from _link_ tags

 - `-includeImages` Whether to include the "src" attribute from _img_ tags
 
 - `-includeFailureCheck` Whether to check for the Katelyn Error Comment `<!-- KATELYN:ERRORS(1) -->`

 - `-includeRobots` Whether to look for the robots.txt file and follow the sitemap linked within it

 - `-maxDepth` The maximum depth to crawl to the site

 - `-delay` If specified, the number of milliseconds to pause before each request

 - `-searchExpression` If specified, a regular expression that fails a page if matched

 - `-partnerSites` A comma-separated list of addresses that can be treated as "internal" _this should only be websites you own_

For example, with everything specified:

    dotnet Katelyn.dll CrawlWith
        -address=http://localhost:51746/ 
        -verbose=true
        -maxDepth=100
        -includeLinks=true
        -includeScripts=true 
        -includeStyles=true 
        -includeImages=true
        -includeFailureCheck=true
		-includeRobots=true
        -delay=500
        -searchExpression="(?i)example[\w\s]expression"
		-partnerSites="http://example.com,http://www.example.com"

This example will crawl the website to 100 levels deep, including all resources, 
with a 500 millisecond delay between receiving a response and making the next request.

Or use the Katelyn Core library in your own applications.

## Done

 - Link discovery from a supplied root address
 - Recursive crawling using found links
 - One crawl per address to avoid repeating a check
 - Media type detection
 - Ensure memory profile remains below 50MB during operation
 - Expose max depth to caller
 - Find other resources (css, scripts, images) and check them
 - Ignore document hashes (i.e. crawl the page, ignoring the location hash)
 - Expose a crawl delay to caller
 - Method of indicating a partial failure (i.e. one module failed, but a page is still being served)
 - Multi-domain service to slow-crawl a list of domains (supply a FilePath to a file with one line per domain)
 - Find robots.txt
 - Follow sitemap link from robots.txt
 - Report on third party resources that have been excluded from the crawl
 - Partner site concept (i.e. where you use a cookieless domain for images and want them treated as internal, rather than third party resources)
 - Stop the crawler part-way through a crawl from the UI
 - Collect only interesting results in the UI (Errors Only)
