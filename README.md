### Aprimo's Open Source Policy 
This code is provided by Aprimo _as-is_ as an example of how you might solve a specific business problem. It is not intended for direct use in Production without modification.

You are welcome to submit issues or feedback to help us improve visibility into potential bugs or enhancements. Aprimo may, at its discretion, address minor bugs, but does not guarantee fixes or ongoing support.

It is expected that developers who clone or use this code take full responsibility for supporting, maintaining, and securing any deployments derived from it.

If you are interested in a production-ready and supported version of this solution, please contact your Aprimo account representative. They can connect you with our technical services team or a partner who may be able to build and support a packaged implementation for you.

Please note: This code may include references to non-Aprimo services or APIs. You are responsible for acquiring any required credentials or API keys to use those services—Aprimo does not provide them.

# Classification-Importer-Tool

This simple command line tool can be used to import classifications into DAM in bulk, using Excel CSV file. The CSV that is used as import file is using ; as separator. 
This is what the tool can do:
- Create new classification objects in DAM or update existing classifications
- For each new classification you’re importing you will be able to define
   - Parent classification namepath
   - New Classification name
   - Identifier
   - Label in default language (English)
   - A number of fields and their values for classification object – intended to support classifications that have dependencies configured

 For example CSV file and full instructions about how to run the tool, please refer to files within folder Example CSV. 
# Open Source Policy

For more information about Aprimo's Open Source Policies, please refer to
https://community.aprimo.com/knowledgecenter/aprimo-connect/aprimo-connect-open-source

**Disclaimer: This tool is not productized Aprimo tool, therefore Aprimo product team does not guarantee for the results nor can you report issues found with the tool to Aprimo Service Now portal. This is an open source tooling developed over time by Aprimo consultants and has been successfully utilized during Aprimo Services team led Activations**
