import React from 'react';
import ReactDOM from 'react-dom';
//import Popup from 'reactjs-popup';

const strElement = <div><h1>Hello React! H1 element added</h1> <li>peugeot</li> <li>citreon</li> <li>renault</li></div>

class Garage extends React.Component
{
	even = (a,b) =>{
		alert(b.type);
		//here a and b are reversed
	}
	render()
	{
		//here using bind function i dunno what it instead of arrow function
		return (
		<div onClick={this.even.bind(this,"goal")}>
			<h1>this is the garage class inside car class</h1>
		</div>
		);
	}
	componentWillUnmount()
	{
		alert("garage component unmounted");
	}
}

class Car extends React.Component
{
	constructor(props)
	{
		super(props);
		this.state = {dd: "tata", strVar: "", err: "",headr: "", nam:"", age: '', bleh: "red", col: "EHEH", model: props.col.model, show: true};
		this.innerHTML = "inner HTML text :O"
	};
	changeColor =() =>{
		if(this.state.bleh == "red")this.setState({bleh: "BLUE"});
		if(this.state.bleh == "BLUE")this.setState({bleh: "red"});
	}
	delThis = () => {
		this.setState({show: false});
	}
	/*static getDerivedStateFromProps(props,state)
	{
		return {model: props.col.model};
	}*/
	Aler = () => {
		alert("teehee an alert box :)");
	}
	render()
	{
		let x;
		if(this.state.show == true)
		{
			x=<Garage />
		}
		let head="";
		if(this.state.headr || this.state.nam || this.state.age){
			head = <p>hello {this.state.nam}, you are here for {this.state.headr} and you are {this.state.age} years old</p>;
		}
		
		return (
			<div>
				{x}
				<p>this paragraph of {this.state.bleh} car eheh</p>
				<p>second para {this.innerHTML}</p>
				<p>passing car details as a prop: {this.props.col.brand}, {this.props.col.model}</p>
				<p>same variable name as prop but in state {this.state.col}</p>
				<p>details through getDerivedStateFromProps: {this.state.bleh}, {this.state.col}, <b>{this.state.model}</b></p>
				<br/>
				<p>issa {this.state.bleh} color car</p>
				<button type="button" onClick={this.changeColor}>change colour</button>
				<button type="button" onClick={this.delThis}>delete/unmount garage component</button>
				<button type="button" onClick={this.Aler}>Shows Alert Box</button>
				<div id="boo"></div>
				<div id="deb"></div>
				<div id="up"></div>
				<form>
					<h1>This is a form :)</h1>
					{head}
					<p>Enter your headr:</p>
					<input type="text" name="headr" onChange={this.formHandler}/>
					<p>Enter your name:</p>
					<input type="text" name="nam" onChange={this.formHandler}/>
					<p>Enter your age:</p>
					<input type="text" name="age" onChange={this.formHandler}/>
					<p>A text area which is now stored as a state variable in react: </p>
					<textarea name="txt" value={this.state.strVar}/>
					<p>Here's a drop down list :)</p>
					<select value={this.state.dd}>
						<option value="mahindra">Mahindra</option>
						<option value="bajaj">Bajaj</option>
						<option value="tata">TATA</option>
						<option value="tvs">TVS</option>
					</select>
					<p></p>
					<input type="submit"/>
					<p>{this.state.err}</p>
				</form>
			</div>
		);
	}
	componentDidMount(){
		setTimeout(() => {this.setState({model: 2000})}, 2000); //setState gets overridden by getDerivedStateFromProps. pass props in constructor instead
		document.getElementById("deb").innerHTML = "componentDidMount executed";
	}
	getSnapshotBeforeUpdate(prevProps,prevState)
	{
		document.getElementById("boo").innerHTML = "before the update value was: "+prevState.bleh;
	}
	componentDidUpdate()
	{
		document.getElementById("up").innerHTML += "*";
	}
	formHandler = (event) =>{
		/* document.getElementById("up").innerHTML += "FORM updated!<br>"; */
		let n = event.target.name;
		let v = event.target.value;
		if(n === "age" && v!=0 && !Number(v))this.setState({err: "age must be number"});
		else this.setState({[n]: v});
	}
}
class App extends React.Component
{
	
	render()
	{
		const cars = {brand: "Ford", model: 1998};
		return(ReactDOM.render(<Car col={cars}/>,document.getElementById('root')))
	}
}

export default App;