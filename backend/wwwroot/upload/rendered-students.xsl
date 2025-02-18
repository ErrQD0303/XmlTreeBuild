<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/class">
		<html>
			<body>
				<h2>
					Student List
				</h2>
				<table border="1" style="width:100%">
					<tr style="background-color: lightblue;">
						<th>
							First Name
						</th>
						<th>
							Last Name
						</th>
						<th>
							Nick Name
						</th>
					</tr>
					<xsl:for-each select="student">
						<tr>
							<td>
								<xsl:value-of select="firstname">
								</xsl:value-of>
							</td>
							<td>
								<xsl:value-of select="lastname">
								</xsl:value-of>
							</td>
							<td>
								<xsl:value-of select="nickname">
								</xsl:value-of>
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
	<!-- Match class element in xml file -->
</xsl:stylesheet>