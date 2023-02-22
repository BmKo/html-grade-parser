# HTML Grade Parser
This will allow you to send the HTML document that the VUW ECS Assessment Marks page is using and it will scrape your grades returning them in the JSON format.

# How it will work
It will be a POST endpoint you can hit with an HTML document that has been compressed with GZip and encoded as Base64. It will decode and decompress the message then scrape
the grade information returning into an easy to use JSON response.

You can compress the HTML document using this [online compressor](https://facia.dev/tools/compress-decompress/gzip-compress/) 
