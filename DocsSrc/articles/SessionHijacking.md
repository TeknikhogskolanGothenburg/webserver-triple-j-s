# Session hijacking
Describe the cookie vulnerability called "Session hijacking" which the archicture of this webserver contains. Both the problem and a possible solution.

Session hijacking (theft) of a user Cookie from a browser:
is to illegally take over someone else's session on the internet while it is running. The session hijacker takes over the identity of the victim without the website noticing anything. The hijacker can then enter false information, change settings and make orders in the victim's place. Session creation requires the hijacker to find out the victim's session ID.

The solution could be using SSL or the more updated TLS (Transport Layer Security) visible in the URL as HTTPS.

SSL stands for Secure Sockets Layer and it's the standard technology for keeping an internet connection secure and safeguarding any sensitive data that is being sent between two systems, preventing criminals from reading and modifying any information transferred, including potential personal details. It does this by making sure that any data transferred between users and sites, or between two systems remain impossible to read. It uses encryption algorithms to scramble data in transit, preventing hackers from reading it as it is sent over the connection. This information could be anything sensitive or personal which can include credit card numbers and other financial information, names and addresses.

HTTPS (Hyper Text Transfer Protocol Secure) appears in the URL when a website is secured by an SSL certificate. The details of the certificate, including the issuing authority and the corporate name of the website owner, can be viewed by clicking on the lock symbol on the browser bar.
