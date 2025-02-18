function Home() {
  return (
    <>
      <div>Homepage</div>

      <p>File Upload</p>

      <div>Whole XML</div>
      <form
        action="https://localhost:7224/api/xml/fileupload"
        method="post"
        encType="multipart/form-data"
      >
        <input type="file" name="file" />
        <button type="submit">Upload</button>
      </form>
      <div>Prolog Only</div>
      <form
        action="https://localhost:7224/api/xml/PrologParse"
        method="post"
        encType="multipart/form-data"
      >
        <input type="file" name="file" />
        <button type="submit">Upload ProLog Only</button>
      </form>
      <div>XML Declaration Only</div>
      <form
        action="https://localhost:7224/api/xml/XMLDeclarationParse"
        method="post"
        encType="multipart/form-data"
      >
        <input type="file" name="file" />
        <button type="submit">Upload XML Declaration Only</button>
      </form>
    </>
  );
}

export default Home;
