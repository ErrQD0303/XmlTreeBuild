@echo off

set models=Models/XMLAttribute,Models/XMLElement,Models/XMLEmentList,Models/XMLDocument,XMLParser,Exceptions/XMLParserError,Helpers/XMLUtils

for %%f in (%models%) do (
  start cmd /c "type nul > %%f.cs && exit"
)

type nul > GlobalUsings.cs