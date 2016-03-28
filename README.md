# Katelyn

Well known for Crawling.

Katelyn is a simple slow-crawler for checking a single root website for crawl errors by following links discovered during the crawl.
The "slow cralwer" is designed to make light demands on your web server, making no concurrent requests. Additional delay can be added 
to make the crawl even slower if necessary.

You can run Katelyn from the command line:

    Katelyn Crawl -address=http://localhost/

Or

    Katelyn Crawl -address=http://localhost/ -verbose=false

Basic Options

 - `-address` is mandatory. Specify the full URI to be used as the root for the crawling session

 - `-verbose` optional, if `true` shows information for every URI crawled

When using only the basic options, the recommended defaults are used for all other settings.
By default, Katelyn crawls all links, scripts, styles, and images that belong to the root
address and ignored any external resources. The crawl is performed one request at a time, with
no additional delay.

If you wish to specify these options manually, you can use the advanced options along with the `CrawlWith` method:

    Katelyn CrawlWith ...

Advanced Options

 - `-address` is mandatory. Specify the full URI to be used as the root for the crawling session

 - `-verbose` optional, if `true` shows information for every URI crawled

 - `-includeLinks` Whether to include the "href" attribute from _a_ tags

 - `-includeScripts` Whether to include the "src" attribute from _script_ tags

 - `-includeStyles` Whether to include the "href" attribute from _link_ tags

 - `-includeImages` Whether to include the "src" attribute from _img_ tags

 - `-maxDepth` The maximum depth to crawl to the site

 - `-delay` If specified, the number of milliseconds to pause before each request

For example, with everything specified:

    Katelyn CrawlWith
        -address=http://localhost/ 
        -verbose=true
        -maxDepth=100
        -includeLinks=true
        -includeScripts=true 
        -includeStyles=true 
        -includeImages=true
        -delay=500

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

## TODO

 - Sample listeners (i.e. dogstatsd, eventlog)
 - Multi-domain service to slow-crawl a list of domains