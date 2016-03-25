# Katelyn

Well known for Crawling.

Katelyn is a simple slow-crawler for checking a single website for crawl errors by following links out from the home page.

You can run Katelyn from the command line:

    Katelyn Crawl -address=http://localhost/ -maxDepth=10

Options

 - -address The URI of the root of the website

 - -maxDepth The maximum depth to crawl to the site

 - -includeLinks Whether to include the "href" attribute from _a_ tags (true by default)

 - -includeScripts Whether to include the "src" attribute from _script_ tags (false by default)

 - -includeStyles Whether to include the "href" attribute from _link_ tags (false by default)

 - -includeImages Whether to include the "src" attribute from _img_ tags (false by default)

For example:

    C:\Code\GitHub\Repos\Katelyn\Katelyn\Katelyn.Console\bin\Debug>Katelyn Crawl 
        -address=http://localhost/ 
        -maxDepth=5 
        -includeScripts=true 
        -includeStyles=true 
        -includeImages=true

Or use the Katelyn Core library in your own applications.

## Done

 - Link discovery from a supplied root address
 - Recursive crawling using found links
 - One crawl per address to avoid repeating a check
 - Media type detection
 - Ensure memory profile remains below 50MB during operation
 - Expose max depth to caller
 - Find other resources (css, scripts, images) and check them

## TODO

 - Expose a crawl delay to caller
 - Sample listeners (i.e. dogstatsd, eventlog)
 - Multi-domain service to slow-crawl a list of domains