How to enable fubu behavior chains in an Asp.Net MVC application
--

1. Create a Registry class (ie, SampleRegistry) that inherits from
MvcToFubuRegistry.

1. In your global.asax in the Application_Start make a call to

`MvcToFubuApplication.Start<SampleRegistry>(ObjectFactory.Container);`
