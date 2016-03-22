# Katelyn

Well known for Crawling.

Katelyn is a simple slow-crawler for checking a single website for crawl errors by following links out from the home page.

You can run Katelyn from the command line:

    Katelyn Crawl -address=http://localhost/ -maxDepth=10

Or use the Katelyn Core library in your own applications.

## Done

 - Link discovery from a supplied root address
 - Recursive crawling using found links
 - One crawl per address to avoid repeating a check
 - Media type detection
 - Ensure memory profile remains below 50MB during operation
 - Expose max depth to caller

## TODO

 - Find other resources (css, scripts, images) and check them
 - Expose a crawl delay to caller
 - Sample listeners (i.e. dogstatsd, eventlog)
 - Multi-domain service to slow-crawl a list of domains