## GRVAS Ceu Email Trigger

This is an AWS Lambda function, triggered using EventBridge to run monthly. The lambda scrapes several websites to gather EMT class information for the current month, formats the information in an email, and sends the email to a designated reciever.

An AWS account is required to run this, though the services required currently fall under the free tier. You will also need an email address which has been verified for aws SES in order to send your emails.

Requirements for testing and deploying: You will need to install node, .net7 (or greater), serverless, the aws cli, and aws-lambda. 


To (re)deploy this lambda to aws, run the following from your terminal, opened to the folder containing the serverless.yml folder:
`npm run build`
`serverless deploy --stage dev --aws-profile default`

Once deployed, the lambda will autimatically run at 9am on the first day of every month. This can be changed by editing the cron setting in the serverless.yaml file.

In order for this lambda to run successfull, 2 environment variables must be added. I suggest adding through the aws Lambda console, however you can also hard code them if you want to.
#Required Environment variables:
- DESTINATION_EMAIL
- SENDER_EMAIL
- FAILURE_EMAIL
