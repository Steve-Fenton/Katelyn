# Katelyn

Well known for Crawling.

Katelyn is a simple slow-crawler for checking a single website for crawl errors by following links out from the home page.

You can run Katelyn from the command line:

    Katelyn Crawl -address=http://localhost/ -includeLinks=true -maxDepth=10

Options

 - -address The URI of the root of the website


 - -includeLinks Whether to include the "href" attribute from _a_ tags

 - -includeScripts Whether to include the "src" attribute from _script_ tags

 - -includeStyles Whether to include the "href" attribute from _link_ tags

 - -includeImages Whether to include the "src" attribute from _img_ tags

 - -maxDepth The maximum depth to crawl to the site

 - -verbose Whether to log success messages as well as errors

 - -delay If specified, the number of milliseconds to pause before each request

For example:

    Katelyn Crawl 
        -address=http://localhost/ 
        -maxDepth=5 
        -includeLinks=true
        -includeScripts=true 
        -includeStyles=true 
        -includeImages=true
        -verbose=true
        -delay=500

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

## TODO

 - Expose a crawl delay to caller
 - Sample listeners (i.e. dogstatsd, eventlog)
 - Multi-domain service to slow-crawl a list of domains